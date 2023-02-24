using static Microsoft.Maui.ApplicationModel.Permissions;

namespace ACycle.Helpers
{
    public static class PermissionHelper
    {
        public static async Task<PermissionStatus> CheckAndRequestPermission<TPermission>() where TPermission : BasePermission, new()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<TPermission>();

            if (status == PermissionStatus.Granted)
                return status;

            status = await Permissions.RequestAsync<TPermission>();

            return status;
        }
    }
}
