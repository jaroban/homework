using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Writers
{
    public interface IFileWriter
    {
        Task WriteFile(string path, string content);
    }
}
