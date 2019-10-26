using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vlerx.Es.DataStorage;
using Vlerx.Es.EventMapping.Serialization;
using Vlerx.Es.EventStore;
using Vlerx.Es.EventStore.Subscription;
using Vlerx.Es.Messaging;
using Vlerx.Es.Persistence;
using Vlerx.Es.StoryBroker;
using Vlerx.InternalMessaging;
using Vlerx.SampleContracts.Customers;
using Vlerx.SampleReadSide;
using Vlerx.SampleService.Customers;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Vlerx.SampleService.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment
            , IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        private IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var esConnection = EventStoreConfiguration.ConfigureEsConnection(
                Configuration["EventStore:ConnectionString"],
                Environment.ApplicationName);
            // services.AddSingleton<IEventStoreConnection>(esConnection);
            IEventSerdes eventSerdes = new ContextEventSchema();
            IEventStorage storage = new EventStorage(esConnection, eventSerdes);
            var stories = Stories.OfUseCases(
                new CustomerUseCases(new Repository<Customer.State>(storage))
            );
            services.AddSingleton(stories);

            var documentStore = RavendbDocumentStoreFactory.CreateDocStore(
                                Configuration["RavenDb:Server"],
                                Configuration["RavenDb:Database"]
                            );
            var customerViewModelWriter =
                new RaventDbAtomicWriter<string, CustomerViewModel>(documentStore);
            services.AddSingleton<IAtomicWriter<string, CustomerViewModel>>(customerViewModelWriter);
            services.AddSingleton(new CustomerEventListener(customerViewModelWriter));
            services.AddSingleton<IListenTo<CustomerRegistered>>(x => x.GetService<CustomerEventListener>());
            services.AddSingleton<IListenTo<CustomerRegistered>>(x => x.GetService<CustomerEventListener>());
            services.AddSingleton<IListenTo<CustomerRelocated>>(x => x.GetService<CustomerEventListener>());
            services.AddSingleton<IListenTo<CustomerContactInfoChanged>>(x => x.GetService<CustomerEventListener>());

            var subscriptionIntegrator = new SubscriptionIntegrator(
                esConnection
                , new EsCheckpointStore(esConnection, "process-checkpoint")
                , "processMessenger"
                // , new ProcessMessenger(
                //     new ProcessOf<SampleProcessState>(
                //             new SampleStateRepository()
                //             , stories
                //         )
                // )
                //or:
                //   , new ProcessMessenger(
                //       new SampleProcess(
                //               new SampleStateRepository()
                //               , stories
                //           )
                //   )
                //or:
                , MessageBroadcaster<EventEnvelope>.Subscribe(
                    services.BuildServiceProvider()
                    , m => m.Payload)
                , eventSerdes
            );

            var eventStoreService = new EventStoreService(
                esConnection,
                subscriptionIntegrator);
            services.AddSingleton<IHostedService>(eventStoreService);

            services.AddSingleton<IQueryReader<CustomerViewModel>>(new RavenDbQueryReader<CustomerViewModel>(documentStore));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Customer}/{action=Register}/{id?}");
            });
        }
    }
}