using System.Reflection;
using System.Web.Http;
using ApiLayer.Interface;
using ApiLayer.Service;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using PersistentLayer.Interface;
using PersistentLayer.Repository;
using StackExchange.Redis;

namespace ApiLayer.App_Start
{
    public class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //註冊 service 跟 repository
            //builder.RegisterType<LoginService>().As<ILoginService>().InstancePerRequest();
            //builder.RegisterType<TaskService>().As<ITaskService>().InstancePerRequest();
            //builder.RegisterType<SessionService>().As<ISessionService>().InstancePerRequest();
            builder.RegisterType<MemberRepository>().As<IMemberRepository>().InstancePerRequest();
            builder.RegisterType<AccountAccessService>().As<IAccountAccessService>().InstancePerRequest();

            // 註冊 Redis 連線為 Singleton
            builder.Register(c =>
            {
                // 這裡使用 "localhost:6379" 作為 Redis 伺服器的位址
                return ConnectionMultiplexer.Connect("localhost:6379");
            }).As<ConnectionMultiplexer>().SingleInstance(); // 使用 SingleInstance 保證同一個連線

            // 註冊 Mapper
            MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>(); // 註冊你的 AutoMapperProfile
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            // 註冊 Web API 控制器
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // 註冊 filter
            //builder.RegisterType<VeriyLoginFilter>().InstancePerRequest();
            //builder.RegisterType<AdminPermissionAuthFilter>().InstancePerRequest();

            // 把 Autofac 的 FilterProvider 自動掛進 config
            HttpConfiguration config = GlobalConfiguration.Configuration;
            builder.RegisterWebApiFilterProvider(config);

            IContainer container = builder.Build();

            // 設定 Autofac 為 Web API 的 DI 容器
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}