using System;
using System.IO;
using System.Reflection;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class MessagingPluginFinder : IPluginFinder
    {
        private readonly IMessagingPlugInRepository _PlugInRepository;

        public MessagingPluginFinder(IMessagingPlugInRepository plugInRepository)
        {
            _PlugInRepository = plugInRepository;
        }

        public void GetAllPlugins()
        {
            var mainExeDir = Environment.CurrentDirectory;
            var pluginDir = new DirectoryInfo(mainExeDir);
            var thisAssembly = Assembly.GetExecutingAssembly();
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly == null) return;

            foreach (var fileInfo in pluginDir.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                if (string.Compare(fileInfo.FullName, thisAssembly.Location, true) == 0)
                    continue;
                if (string.Compare(fileInfo.FullName, entryAssembly.Location, true) == 0)
                    continue;
                try
                {
                    var assembly = Assembly.LoadFrom(fileInfo.FullName);
                    foreach (var type in assembly.GetTypes())
                    {
                        var found = false;
                        foreach (var interfaceType in type.GetInterfaces())
                        {
                            if (interfaceType == typeof(IMessagingService))
                            {
                                _PlugInRepository.Add(type);
                                found = true;
                                break;
                            }
                        }
                        if (found)
                            break;
                    }
                }
                catch (Exception)
                { }
            }
        }
    }
}