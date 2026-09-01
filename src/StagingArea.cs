using System;
using System.IO;
using CilDotNet.Threading;

namespace CilDotNet
{
    public sealed class StagingArea : IDisposable
    {
        private readonly string _basePath;
        private readonly LockFile lockFile;
        private bool _disposed;

        public StagingArea(string StagingPath)
        {
            _basePath = Path.Combine(StagingPath, "StagingArea");
            lockFile = new LockFile(Path.Combine(_basePath, ".lock"));
            EnsureDirectory();
            lockFile.Lock();
        }

        public void Staging(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            var source = path;
            var dest = Path.Combine(_basePath, Path.GetFileName(path));

            if (File.Exists(source))
            {
                File.Copy(source, dest, true);
            }

            else if (Directory.Exists(source))
            {
                CopyDirectory(source, dest);
            }

            else return;
        }

        public string[] ListStaged()
        {
            return Directory.GetFiles(_basePath, "*", SearchOption.AllDirectories);
        }

        public void Clear()
        {
            foreach (var file in Directory.GetFiles(_basePath, "*", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
            foreach (var dir in Directory.GetDirectories(_basePath))
            {
                Directory.Delete(dir, true);
            }
        }

        private void EnsureDirectory()
        {
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        private static void CopyDirectory(string s, string d)
        {
            Directory.CreateDirectory(d);

            foreach (var file in Directory.GetFiles(s))
            {
                var destFile = Path.Combine(d, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var dir in Directory.GetDirectories(s))
            {
                var destDir = Path.Combine(d, Path.GetFileName(dir));
                CopyDirectory(dir, destDir);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            lockFile?.Dispose();
            _disposed = true;
        }
    }
}