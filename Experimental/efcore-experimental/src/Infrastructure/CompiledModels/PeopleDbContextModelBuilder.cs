﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace Tsmoreland.EntityFramework.Core.Experimental.Infrastructure.CompiledModels
{
    public partial class PeopleDbContextModel
    {
        partial void Initialize()
        {
            var person = PersonEntityType.Create(this);

            PersonEntityType.CreateAnnotations(person);

            AddAnnotation("ProductVersion", "6.0.8");
        }
    }
}