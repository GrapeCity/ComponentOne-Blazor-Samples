using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using BlazorExplorer.Localization;

namespace BlazorExplorer.Models
{
    public static class ControlPages
    {
        private static List<ControlPageGroup> _pageGroups;
        private static List<ControlPage> _pages;
        private static List<ControlGroup> _controlGroups;
        private static IDictionary<string, ControlPage> _pageDic;
        private static readonly object _locker = new object();
        private static ControlGroup _popularGroup;
        private static ControlGroup _newGroup;
        private static ControlGroup _refAppGroup;

        //public static _BreadcrumbNav _BreadcrumbNav;

        public static string MapPath
        {
            get
            {
				return "";// Startup.Environment.ContentRootPath;
            }
        }


    public static string GetFileResContent(string filePath)
    {
        var absFilePath = Path.Combine(MapPath, filePath);
        if (File.Exists(absFilePath))
        {
            return GetFileAsHtmlContent(absFilePath);
        } else
        {
            var resPath = filePath.Replace('/', '.');
        return GetResourceContent(resPath, false);
        }
    }

    public static IDictionary<string, string> GetPageSources(string controllerName, string actionName)
		{
		    var pageSources = new Dictionary<string, string>();

		    string key, sourceCode;
		    List<string> filePaths = new List<string>
		    {
				    string.Format("{0}/{1}/{2}{3}", "Pages", controllerName, actionName, ".razor"),
				    string.Format("{0}/{1}/{2}{3}", "Controllers", controllerName, actionName, ".cs"),
				    string.Format("{0}/{1}/{2}{3}", "Models", controllerName, actionName, ".cs")
		    };

        /*
        string folderPath = string.Format("{0}/{1}", "Models", controllerName);
        var files = Directory.GetFiles(folderPath, "*.cs", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
          filePaths.Add(file.Replace("\\","/"));
        */
        
        foreach (var filePath in filePaths)
		    {
			      sourceCode = GetFileResContent(filePath);
			      if (!String.IsNullOrEmpty(sourceCode))
			      {
			        key = filePath.Substring(filePath.LastIndexOf("/") + 1);
              if (pageSources.ContainsKey(key))
                  key = filePath;
			        pageSources.Add(key, sourceCode);
			      }
		    }
		    return pageSources;
		}

	      public static ControlPageGroup GetControlPageGroup(string controlName)
        {
            EnsurePages();
            return _pageGroups.FirstOrDefault(pageGroup => pageGroup.ControlName.ToLower() == controlName.ToLower());
        }

        public static ControlPage GetControlPage(string controlName, string actionName)
        {
            var group = GetControlPageGroup(controlName);
            if(group == null)
            {
                return null;
            }

            actionName = string.IsNullOrEmpty(actionName) ? "Index" : actionName;
            return group.GetPage(actionName);
        }

        private static string GetFileAsHtmlContent(string controllerFilePath)
        {
			try
			{
				return System.IO.File.ReadAllText(controllerFilePath);
            }
            catch (IOException)
            {
				return null;
			}            
        }

        public static string GetPageText(string controllerName, string actionName)
        {
            EnsurePages();
            var key = CreatePageKey(controllerName, actionName);
            return _pageDic.ContainsKey(key) ? _pageDic[key].Text : "Overview";
        }

        private static string CreatePageKey(string controllerName, string actionName)
        {
            return string.Format("{0}-{1}", controllerName, actionName);
        }

        public static IEnumerable<ControlPage> Pages
        {
            get
            {
                EnsurePages();
                return _pages;
            }
        }

        public static ControlGroup GetGroup(string name)
        {
            return ControlGroups.FirstOrDefault(g => string.Equals(g.GroupName, name, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<ControlGroup> ControlGroups
        {
            get
            {
                EnsurePages();
                return _controlGroups;
            }
        }

        public static IEnumerable<ControlGroup> VisibleControlGroups
        {
            get
            {
                return ControlGroups.Where(g => g.Visible);
            }
        }

        private static void EnsurePages()
        {
            if (_pages != null)
            {
                return;
            }

            lock (_locker)
            {
                if (_pages != null)
                {
                    return;
                }

                using (var stream = GetPageConfigStream())
                {
                    var root = XElement.Load(stream);
                    _controlGroups = new List<ControlGroup>();
                    _pages = new List<ControlPage>();
                    _pageGroups = new List<ControlPageGroup>();
                    _newGroup = new ControlGroup 
                    { 
                      GroupNameEn = "New Controls",
                      GroupNameJp = Localization.Resource.NewControls
                    };
                    _popularGroup = new ControlGroup 
                    { 
                      GroupNameEn = "Popular Controls",
                      GroupNameJp = Localization.Resource.PopularControls
                    };
                    _refAppGroup = new ControlGroup
                    {
                        GroupNameEn = "Reference Applications",
                        GroupNameJp = Localization.Resource.Reference_Applications
                    };
                    foreach (var controlElement in root.Elements("ControlGroup"))
                    {
                        var pageGroups = new List<ControlPageGroup>();
                        var visibleAttr = controlElement.Attribute("visible");
                        var visible = visibleAttr == null || visibleAttr.Value != "false";
                        ControlGroup controlGroup = new ControlGroup
                        {
                            GroupNameEn = controlElement.Attribute("name").Value,
                            GroupNameJp = controlElement.Attribute("name.ja")?.Value,
                            Visible = visible,
                            Controls = pageGroups
                        };

                        foreach (var pageElement in controlElement.Elements("Control"))
                        {
                            var currentControl = pageElement.Attribute("name").Value;
                            var currentControlJa = pageElement.Attribute("name.ja")?.Value;
                            var docElement = pageElement.Attribute("documentation");
                            var documentation = docElement == null ? null : docElement.Value;
                            var documentationJa = pageElement.Attribute("documentation.ja")?.Value;
                            var linkAttr = pageElement.Attribute("link");
                            var linkJa = pageElement.Attribute("link.ja")?.Value;
                            var newAttr = pageElement.Attribute("new");
                            var isNew = false;
                            if (newAttr != null)
                            {
                                bool.TryParse(newAttr.Value, out isNew);
                            }

                            var popularAttr = pageElement.Attribute("popular");
                            var popular = false;
                            if (popularAttr != null)
                            {
                                bool.TryParse(popularAttr.Value, out popular);
                            }

                            var enhancedAttr = pageElement.Attribute("enhanced");
                            var isEnhanced = false;
                            if (enhancedAttr != null)
                            {
                                bool.TryParse(enhancedAttr.Value, out isEnhanced);
                            }
                            var sampleAppAttr = pageElement.Attribute("referenceApplication");
                            var isReferenceApplication = false;
                            if (sampleAppAttr != null)
                            {
                                bool.TryParse(sampleAppAttr.Value, out isReferenceApplication);
                            }
                            var textAttr = pageElement.Attribute("text");
                            var textJaAttr = pageElement.Attribute("text.ja");
                            var enhanceTip = pageElement.Attribute("enhancetip")?.Value;
                            var enhanceTipJa = pageElement.Attribute("enhancetip.ja")?.Value;

                            var pages = GetControlPagesFromXEle(pageElement, currentControl);
                            _pages.AddRange(pages);
                            var controlPageGroup = new ControlPageGroup
                            {
                                ControlNameEn = currentControl,
                                ControlNameJp = currentControlJa,
                                ControlGroupName = controlGroup.GroupName,
                                DocumentationEn = documentation,
                                DocumentationJp = documentationJa,
                                Pages = pages,
                                LinkEn = linkAttr != null ? linkAttr.Value : null,
                                LinkJp = linkJa,
                                IsPopular = popular,
                                IsNew = isNew,
                                IsEnhanced = isEnhanced,
                                TextEn = textAttr == null ? currentControl : textAttr.Value,
                                TextJp = textJaAttr == null ? currentControlJa : textJaAttr.Value,
                                EnhanceTipEn = enhanceTip,
                                EnhanceTipJp = enhanceTipJa
                            };

                            if (isNew)
                            {
                                _newGroup.Controls.Add(controlPageGroup);
                            }

                            if (popular)
                            {
                                _popularGroup.Controls.Add(controlPageGroup);
                            }

                            if (isReferenceApplication)
                            {
                                _refAppGroup.Controls.Add(controlPageGroup);
                                controlGroup.Visible = false;
                            }
                            else
                            {
                                pageGroups.Add(controlPageGroup);
                            }
                        }
                        _pageGroups.AddRange(pageGroups);
                        _controlGroups.Add(controlGroup);
                    }

                    _pageDic = new Dictionary<string, ControlPage>(StringComparer.OrdinalIgnoreCase);
                    EnsurePageDic(_pages);
                }
            }
        }

        private static void EnsurePageDic(IEnumerable<ControlPage> pages)
        {
            foreach (var page in pages)
            {
                var key = CreatePageKey(page.ControlName, page.Name);
                if (!_pageDic.ContainsKey(key))
                {
                    _pageDic.Add(key, page);
                }

                if (page.SubPages != null)
                {
                    EnsurePageDic(page.SubPages);
                }
            }
        }

        private static Stream GetPageConfigStream()
        {
            return GetResourceStream("ControlPages.xml");
        }

        private static Stream GetResourceStream(string name, bool throwExceptionIfNotFound = true)
        {
            var assembly = typeof(ControlPages).GetTypeInfo().Assembly;
            var res = assembly.GetManifestResourceNames().Where(resName => resName.Contains(name)).ToList();
            if (res.Count == 0)
            {
                if (throwExceptionIfNotFound)
                  throw new ArgumentOutOfRangeException("name");
                return null;
            }
            return assembly.GetManifestResourceStream(res[0]);
        }

        private static string GetResourceContent(string name, bool throwExceptionIfNotFound = true)
    {
            using (var stream = GetResourceStream(name, throwExceptionIfNotFound))
            {
                if (stream != null) 
                { 
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return null;
        }

        private static List<ControlPage> GetControlPagesFromXEle(XElement controlElement, string controlName)
        {
            var pages = controlElement.Elements("action").Select(e =>
            {
                var enhancedAttr = e.Attribute("enhanced");
                var isEnhanced = false;
                if (enhancedAttr != null)
                {
                    bool.TryParse(enhancedAttr.Value, out isEnhanced);
                }

                ControlPage page = new ControlPage
                {
                    TextEn = e.Attribute("text").Value,
                    TextJp = e.Attribute("text.ja")?.Value,
                    Name = e.Attribute("name") != null ? e.Attribute("name").Value : "",
                    ControlName = controlName,
                    IsEnhanced = isEnhanced,
                    EnhanceTipEn = e.Attribute("enhancetip")?.Value,
                    EnhanceTipJp = e.Attribute("enhancetip.ja")?.Value
                };
                if (e.Element("subactions") != null)
                {
                    List<ControlPage> subPages = GetControlPagesFromXEle(e.Element("subactions"), controlName);
                    if (subPages.Any())
                    {
                        page.SubPages = subPages;
						foreach (var subPage in subPages)
						{
							subPage.ParentPage = page;
						}
                    }
                }

                return page;
            }).ToList();
            return pages;
        }


        public static List<NavItem> GetNavMenuItems(IEnumerable<ControlPage> pages, ControlPage currentPage = null, bool isRoot = true, NavItem parentItem = null)
        {
            List<NavItem> items = new List<NavItem>();
            bool isCollapsed = !isRoot;
            foreach (var page in pages)
            {
              NavItem item = new NavItem();
              item.Header = page.Text;
              item.Href = page.Path;
              item.IsCollapsed = isCollapsed;
              isCollapsed = true;
              if (page.SubPages != null && page.SubPages.Any())
              {
                item.Items = GetNavMenuItems(page.SubPages, currentPage, false, item); 
              }
			  item.ParentItem = parentItem;
              item.ParentItems = isRoot ? items : null;	
			  item.Page = page;
              items.Add(item);
			  if (currentPage != null && page == currentPage)
				NavItem.SelectedItem = item;
            }
            return items;
        }
			
        public static List<NavItem> NavItems;
    
        public static ControlGroup NewGroup 
        {
            get
            {
                EnsurePages();
                return _newGroup;
            }
        }

        public static ControlGroup PopularGroup
        {
            get
            {
                EnsurePages();
                return _popularGroup;
            }
        }

        public static ControlGroup ReferenceApplicationGroup
        {
            get
            {
                EnsurePages();
                return _refAppGroup;
            }
        }
    }

    public class ControlGroup
    {
        public ControlGroup()
        {
            Visible = true;
            Controls = new List<ControlPageGroup>();
        }

        public IList<ControlPageGroup> Controls { get; set; }
        internal string GroupNameEn { get; set; }
        internal string GroupNameJp { get; set; }
        public string GroupName
        {
            get
            {
                return IsJpCulture ? GroupNameJp ?? GroupNameEn : GroupNameEn;
            }
        }
        public bool Visible { get; set; }

        internal static bool IsJpCulture
        {
            get
            {
                var culture = System.Globalization.CultureInfo.CurrentUICulture;
                return string.Equals(culture.TwoLetterISOLanguageName, "ja", StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    public class ControlPageGroup
    {
        private string DocumentationRoot = Resource.ControlLayout_DocumentationUrl;

        internal string DocumentationEn { get; set; }
        internal string DocumentationJp { get; set; }
        public string Documentation
        {
            get
            {
                if (ControlGroup.IsJpCulture)
                {
                    return DocumentationJp ?? DocumentationRoot;
                }
                else
                {
                    return DocumentationEn ?? DocumentationRoot;
                }
            }
        }

        public IEnumerable<ControlPage> Pages { get; set; }
        public string ControlNameEn { get; set; }
        internal string ControlNameJp { get; set; }
        public string ControlName
        {
            get
            {
                return ControlGroup.IsJpCulture ? ControlNameJp ?? ControlNameEn : ControlNameEn;
            }
        }
        public string ControlGroupName { get; set; }
        internal string TextEn { get; set; }
        internal string TextJp { get; set; }
        public string Text
        {
            get
            {
                return ControlGroup.IsJpCulture ? TextJp ?? TextEn : TextEn;
            }
        }
        public ControlPage GetPage(string actionName)
        {
            return GetPage(Pages, actionName);
        }

        private ControlPage GetPage(IEnumerable<ControlPage> pages, string name)
        {
            foreach (var page in pages)
            {
                if (string.Equals(page.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return page;
                }

                if (page.SubPages != null)
                {
                    var subPage = GetPage(page.SubPages, name);
                    if (subPage != null)
                    {
                        return subPage;
                    }
                }
            }

            return null;
        }

        internal string LinkEn { get; set; }
        internal string LinkJp { get; set; }
        public string Link
        {
            get
            {
                return ControlGroup.IsJpCulture ? LinkJp ?? LinkEn : LinkEn;
            }
        }

        public bool IsNew { get; set; }

        public bool IsPopular { get; set; }

        public bool IsEnhanced { get; set; }
        internal string EnhanceTipEn { get; set; }
        internal string EnhanceTipJp { get; set; }
        public string EnhanceTip
        {
            get
            {
                return ControlGroup.IsJpCulture ? EnhanceTipJp ?? EnhanceTipEn : EnhanceTipEn;
            }
        }
    }

    public class ControlPage
    {
        public string Name { get; set; }
        internal string TextEn { get; set; }
        internal string TextJp { get; set; }
        public string Text
        {
            get
            {
                return ControlGroup.IsJpCulture ? TextJp ?? TextEn : TextEn;
            }
        }
        public string Path { get; set; }
        public string ControlName { get; set; }
        public bool IsEnhanced { get; set; }
        internal string EnhanceTipEn { get; set; }
        internal string EnhanceTipJp { get; set; }
        public string EnhanceTip
        {
            get
            {
                return ControlGroup.IsJpCulture ? EnhanceTipJp ?? EnhanceTipEn : EnhanceTipEn;
            }
        }
        public List<ControlPage> SubPages { get; set; }
		public ControlPage ParentPage { get; set; }
  }
}