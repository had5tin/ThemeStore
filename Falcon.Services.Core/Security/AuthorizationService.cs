/// <copyright file="AuthorizationService.cs" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description></description>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Security;
using Falcon.Data.Domain;
using Falcon.Data.Repository;
using Falcon.Services.Users;

namespace Falcon.Services.Security
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public AuthorizationService(IRoleRepository roleRepository,
                                    IResourceRepository resourceRepository,
                                    IPermissionRepository permissionRepository,
                                    IUserRepository userRepository,
                                    IUserRoleRepository userRoleRepository)
        {
            this._roleRepository = roleRepository;
            this._resourceRepository = resourceRepository;
            this._permissionRepository = permissionRepository;
            this._userRepository = userRepository;
            this._userRoleRepository = userRoleRepository;
        }

        public bool isAllowed(int userId, string resourceName, string privilege)
        {
            User user = _userRepository.Table.SingleOrDefault(u => u.Id == userId);
            if (user == null) return false;

            return isAllowed(user, resourceName, privilege);
        }

        public bool isAllowed(User user, string resourceName, string privilege)
        {
            Resource resource = GetResource(resourceName);
            if (resource == null) return false;

            return isAllowed(user, resource, privilege);
        }

        public bool isAllowed(User user, Resource resource, string privilege)
        {            
            IEnumerable<UserRole> userRoles = from ur in _userRoleRepository.Table 
                                              where ur.UserId == user.Id 
                                              select ur;
            bool result = false;
            foreach (UserRole userRole in userRoles.ToList())
            {
                result = isAllowed(userRole.RoleId, resource.Id, privilege);
                if (result) break;
            }
            return result;
        }

        #region Roles
        public IEnumerable<Role> GetAllRoles()
        {
            return _roleRepository.Table.ToList();
        }

        public void AddRole(Role role)
        {
            _roleRepository.Add(role);
        }

        public void UpdateRole(Role role)
        {
            Role current = GetRole(role.Id);
            current.Name = role.Name;
            current.ParentId = role.ParentId;
            current.Description = role.Description;

            _roleRepository.SubmitChanges();
        }

        public Role GetRole(int roleId)
        {
            return _roleRepository.Table.SingleOrDefault(r => r.Id == roleId);
        }

        public Role GetRole(string roleName)
        {
            return _roleRepository.Table.SingleOrDefault(r => r.Name == roleName);
        }
        #endregion
        
        #region UserRole
        public IEnumerable<Role> GetAllRolesByUser(int userId)
        {
            var roles = from r in _roleRepository.Table
                        join ur in _userRoleRepository.Table
                        on r.Id equals ur.RoleId
                        where ur.UserId == userId
                        select r;
            return roles.ToList();
        }

        public IEnumerable<User> GetAllUsersByRole(int roleId)
        {
            var users = from u in _userRepository.Table
                        join ur in _userRoleRepository.Table
                        on u.Id equals ur.UserId
                        where ur.RoleId == roleId
                        select u;
            return users.ToList();
        }

        public void AddUserRole(int userId, int roleId)
        {
            _userRoleRepository.Add(new UserRole() { UserId = userId, RoleId = roleId });
        }

        public void RemoveUserRole(int userId, int roleId)
        {
            _userRoleRepository.Remove(new UserRole() { UserId = userId, RoleId = roleId });
        }

        public void RemoveUserRoleByUserId(int userId)
        {
            _userRoleRepository.ExecuteSP("UserRole_DeleteByUserId", new { UserId = userId });
        }
        #endregion

        #region Resources
        public IEnumerable<Resource> GetAllResources()
        {
            return _resourceRepository.Table.ToList();
        }

        public void AddResource(Resource resource)
        {
            _resourceRepository.Add(resource);
        }

        public void UpdateResource(Resource resource)
        {
            Resource current = GetResource(resource.Id);
            current = resource;
            _resourceRepository.SubmitChanges();
        }

        public Resource GetResource(int resourceId)
        {
            return _resourceRepository.Table.SingleOrDefault(r => r.Id == resourceId);
        }

        public Resource GetResource(string resourceName)
        {
            return _resourceRepository.Table.SingleOrDefault(r => r.Name == resourceName);
        }
        #endregion

        #region Permissions
        public IEnumerable<Permission> GetAllPermissions()
        {
            return _permissionRepository.Table.ToList();
        }

        public void AddPermission(Permission permission)
        {
            _permissionRepository.Add(permission);
        }

        public void RemovePermission(Permission permission)
        {
            _permissionRepository.Remove(permission);
        }

        public void UpdatePermission(Permission permission)
        {
            Permission current = GetPermission(permission.RoleId, permission.ResourceId, permission.Privilege);
            current = permission;
            _permissionRepository.SubmitChanges();
        }

        public void UpdateRolePermission(int roleId, IList<Permission> permissions)
        {
            using (var scope = new System.Transactions.TransactionScope())
            {
                try
                {
                    _permissionRepository.ExecuteSP("Permission_DeleteByRoleId", new { RoleId = roleId });
                    foreach (var permission in permissions)
                    {
                        Resource resouce = GetResource(permission.ResourceName);
                        if (resouce == null)
                        {
                            resouce = new Resource()
                            {
                                Name = permission.ResourceName
                            };
                            _resourceRepository.Add(resouce);
                        }
                        permission.ResourceId = resouce.Id;
                        permission.RoleId = roleId;
                        _permissionRepository.Add(permission);
                    }

                    _permissionRepository.SubmitChanges();

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Permission GetPermission(int roleId, int resourceId, string privilege)
        {
            return _permissionRepository.Table.SingleOrDefault(p => p.ResourceId == resourceId && p.RoleId == roleId && p.Privilege == privilege);
        }
        #endregion

        #region Protected Functions
        protected bool isAllowed(Role role, Resource resource, string privilege)
        {
            return isAllowed(role.Id, resource.Id, privilege);
        }

        protected bool isAllowed(int roleId, int resourceId, string privilege)
        {
            Permission permission = GetPermission(roleId, resourceId, privilege);
            if (permission == null) return false;
            return permission.IsAllowed;
        }

        protected bool isAllowed(string roleName, string resourceName, string privilege)
        {
            Role role = _roleRepository.Table.SingleOrDefault(r => r.Name == roleName);
            if (role == null) return false;

            Resource resource = GetResource(resourceName);
            if (resource == null) return false;

            return isAllowed(role.Id, resource.Id, privilege);
        }
        #endregion


        public IEnumerable<Permission_GetByRoleIdResult> GetPermissionsByRoleId(int roleId)
        {
            return _roleRepository.QuerySP<Permission_GetByRoleIdResult>("Permission_GetByRoleId", new { RoleId = roleId }).ToList();
        }
    }
}
