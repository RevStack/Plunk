using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Practices.Unity;
using RevStack.Plunk;
using RevStack.Pattern;
using UnitTest.Models;
using UnitTest.Service;

namespace UnitTest
{
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            string appId = ConfigurationManager.AppSettings[Constants.APPID];
            string accessToken = ConfigurationManager.AppSettings[Constants.AUTHORIZATION];

            container.RegisterType<PlunkDataContext, PlunkDataContext>(new InjectionConstructor(appId, accessToken));

            //repositories
            container.RegisterType<IRepository<Company, int>, PlunkRepository<Company, int>>();
            
            //services
            container.RegisterType<ICompanyService, CompanyService>();
        }
    }
}
