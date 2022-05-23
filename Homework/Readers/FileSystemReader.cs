using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Readers
{
    public class FileSystemReader: IFileReader
    {
        public Task<string> ReadFile(string path)
        {
            return File.ReadAllTextAsync(path);
        }
    }
}
