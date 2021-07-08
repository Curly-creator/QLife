using System;
using System.Collections.Generic;
using System.Text;

namespace QLifeC_Datatool
{
    public class InternetAccess
    {
        private double _downloadSpeed;
        private double _uploadSpeed;
        private double _downloadScore;
        private double _uploadScore;

        //public InternetAccess(double downloadSpeed, double uploadSpeed, double downloadScore, double uploadScore)
        //{
        //    DownloadSpeed = downloadSpeed;
        //    UploadSpeed = uploadSpeed;
        //    DownloadScore = downloadScore;
        //    UploadScore = uploadScore;
        //}

        public double DownloadSpeed { get => _downloadSpeed; set => _downloadSpeed = value; }
        public double UploadSpeed { get => _uploadSpeed; set => _uploadSpeed = value; }
        public double DownloadScore { get => _downloadScore; set => _downloadScore = value; }
        public double UploadScore { get => _uploadScore; set => _uploadScore = value; }
    }
}
