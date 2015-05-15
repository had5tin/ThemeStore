/// <copyright file="WarmupTask.cs" company="DKT">
/// Copyright (c) 2012 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description>Tạo các request tự động đến hệ thống giúp hệ thống được refresh cache dll</description>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Tasks;
using System.Xml;
using System.Web;
using Falcon.Infrastructure;
using Elmah;
using System.Net;
using Common.Logging;
using System.Web.Hosting;

namespace Falcon.Startup
{
    public class WarmupTask : ITask
    {
        private ILog logger = LogManager.GetLogger("WarmupTask");

        public void Execute(XmlNode node)
        {
            //Read warmup.config file, load refresh links
            string strDir = HostingEnvironment.MapPath("~/Config/Warmup.config");
            string domainName = EngineContext.Current.FalconConfig.DomainName;

            if (System.IO.File.Exists(strDir))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(strDir);                    
                    XmlNodeList links = doc.SelectNodes("Links/Link");
                    foreach (XmlNode item in links)
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domainName + item.InnerText);
                        request.Credentials = CredentialCache.DefaultCredentials;
                        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                        WebResponse response = request.GetResponse();
                        response.Close();
                        logger.Info("Auto Request on " + DateTime.Now.ToString() + ":" + domainName + item.InnerText);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }  
        }
    }
}
