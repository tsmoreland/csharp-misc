﻿// <auto-generated />
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.CompiledModels
{
    [DbContext(typeof(PeopleDbContext))]
    public partial class PeopleDbContextModel : RuntimeModel
    {
        static PeopleDbContextModel()
        {
            var model = new PeopleDbContextModel();
            model.Initialize();
            model.Customize();
            _instance = model;
        }

        private static PeopleDbContextModel _instance;
        public static IModel Instance => _instance;

        partial void Initialize();

        partial void Customize();
    }
}