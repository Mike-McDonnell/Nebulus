using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nebulus.Models
{
    public class MessageItem
    {
        public string MessageItemId { get; set; }
        public string MessageTitle { get; set; }
        public string MessageBody { get; set; }
        public MessageType MessageType { get; set; }
        public MessageLocation MessageLocation { get; set; }
        public MessagePriorityType MessagePriority { get; set; }
        public DateTimeOffset ScheduleStart { get; set; }
        public ScheduleIntervalType ScheduleInterval { get; set; }
        public double duration { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string TargetGroup { get; set; }
        public string MessageHeight { get; set; }
        public string MessageWidth { get; set; }
        public string MessageTop { get; set; }
        public string MessageLeft { get; set; }
        public string ADGroupTags { get; set; }

        public virtual bool PassesSecurityFilter
        {
            get
            {
                if (this.ADGroupTags != string.Empty)
                {
                    using (var pC = new System.DirectoryServices.AccountManagement.PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain))
                    {
                        using (var user = System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(pC, System.DirectoryServices.AccountManagement.IdentityType.Sid, System.Security.Principal.WindowsIdentity.GetCurrent().User.Value))
                        {
                            foreach (var group in user.GetGroups())
                            {
                                if (this.ADGroupTags.Contains(group.SamAccountName))
                                {
                                    return true;
                                }

                            }
                        }

                        using (var user = System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(pC, System.DirectoryServices.AccountManagement.IdentityType.Sid, System.Security.Principal.WindowsIdentity.GetCurrent().User.Value))
                        {
                            foreach (var group in user.GetGroups())
                            {
                                if (this.ADGroupTags.Contains(group.SamAccountName))
                                {
                                    return true;
                                }

                            }
                        }

                        //using (var computer = System.DirectoryServices.AccountManagement.ComputerPrincipal.FindByIdentity(pC, System.DirectoryServices.AccountManagement.IdentityType.SamAccountName, Environment.MachineName))
                        //{
                        //    foreach (var group in computer.GetGroups())
                        //    {
                        //        if (this.ADGroupTags.Contains(group.SamAccountName))
                        //        {
                        //            return true;
                        //        }

                        //    }
                        //}

                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
