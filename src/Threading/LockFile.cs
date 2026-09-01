using System;
using System.IO;
using System.Threading;

namespace CilDotNet.Threading
{
    public sealed class LockFile : IDisposable
    {
        private readonly string _filePath;
        private FileStream? _fileStream;
        private bool _locked;

        public LockFile(string filePath)
        {
            _filePath = filePath;
            Lock();
        }

        public void Lock()
        {
            if (_locked) return;

            try
            {
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                _fileStream = new FileStream(
                    _filePath,
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.None
                );
                _locked = true;
            }
            catch
            {
                _locked = false;
            }
        }

        public void Unlock()
        {
            if (!_locked) return;

            try
            {
                _fileStream?.Close();
                _fileStream?.Dispose();
                _fileStream = null;
                _locked = false;

                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }
            }
            catch { }
        }

        public bool FileLocked()
        {
            if (!File.Exists(_filePath)) return false;

            try
            {
                using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                return false;
            }
            catch
            {
                return true;
            }
        }

        public void Dispose()
        {
            Unlock();
        }
    }
}