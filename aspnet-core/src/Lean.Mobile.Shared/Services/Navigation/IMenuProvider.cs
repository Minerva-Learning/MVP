using System.Collections.Generic;
using MvvmHelpers;
using Lean.Models.NavigationMenu;

namespace Lean.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}