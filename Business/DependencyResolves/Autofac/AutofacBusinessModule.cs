using Autofac;
using Business.Abstract;
using Business.Concrette;
using Microsoft.AspNetCore.Http;
using Repository.Abstract;
using Repository.Concrette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolves.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            builder.RegisterType<UserRequestManager>().As<IUserRequestService>();
            builder.RegisterType<UserRequestRepository>().As<IUserRequestRepository>();

            builder.RegisterType<DictionaryManager>().As<IDictionaryService>();
            builder.RegisterType<DictionaryRepository>().As<IDictionaryRepository>();
        
            builder.RegisterType<UserRoleRepository>().As<IUserRoleRepository>();


            builder.RegisterType<SendMessageRepository>().As<ISendMessageRepository>();


        }
    }
}
