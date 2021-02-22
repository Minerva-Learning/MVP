using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lean.Email
{
    public class SiteRootProvider : ISiteRootProvider
    {
        string _siteRoot;
        public SiteRootProvider(string siteRoot)
        {
            _siteRoot = siteRoot;
        }

        public string GetSiteRoot()
        {
            return _siteRoot;
        }
    }
}
