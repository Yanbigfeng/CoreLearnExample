using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearnExample.Filter
{
    public class SkipAttribute : Attribute, IAllowAnonymousFilter
    {
    }
}
