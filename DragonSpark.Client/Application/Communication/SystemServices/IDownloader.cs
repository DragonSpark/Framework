using System;
using System.IO;

namespace DragonSpark.Application.Communication.SystemServices
{
    public interface IDownloader
    {
        Stream Retrieve( Uri uri );
    }
}