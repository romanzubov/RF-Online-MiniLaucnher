using System.Collections.Generic;
using System.Linq;

namespace PatchGenerator.Data
{
    public class FileInfo
    {
        public string Path { get; set; }
        public string Hash { get; set; }
    }

    public static class FileInfoExtensions
    {
        public static IEnumerable<FileInfo> Except(this List<FileInfo> FileInfos, IEnumerable<string> hashes)
        {
            foreach (var file in FileInfos)
            {
                if (!hashes.Contains(file.Hash))
                {
                    yield return file;
                }
            }
        }
    }
}
