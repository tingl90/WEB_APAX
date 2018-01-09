using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Web_Apax.Models
{
    public class clsRoleManager : RoleProvider
    {
        public string[] GetAllRolesByUser(string userName)
        {
            try
            {
                using (APAXEntities3 db = new APAXEntities3())
                {
                    TH_HOSOKHACHHANG Quyen = db.TH_HOSOKHACHHANG.FirstOrDefault(it => it.TAIKHOAN.Equals(userName));
                    if (Quyen != null && Quyen.ID_QUYENNGUOIDUNG>0)
                    {
                        var strQuyen = db.DM_QUYEN.FirstOrDefault(it => it.A_QUYEN == Quyen.ID_QUYENNGUOIDUNG);
                        return strQuyen!=null?strQuyen.QUYENNGUOIDUNG.Split(','):null;
                    }
                    return null;
                }
            }
            catch { return null; }
        }
        public override string[] GetRolesForUser(string userName)
        {
            try
            {
                if (userName.Equals("Administrator"))
                {
                    List<string> Quyen = new List<string>();
                    Quyen.Add("0=0");
                    return Quyen.ToArray();
                }
                using (APAXEntities3 db = new APAXEntities3())
                {
                    TH_HOSOKHACHHANG nguoiDung = db.TH_HOSOKHACHHANG.FirstOrDefault(it => it.TAIKHOAN.Equals(userName));
                    if (nguoiDung != null)
                    {
                        string[] _Quyen = null;
                        List<string> Quyen = new List<string>();
                        string[] QuyenUser = GetAllRolesByUser(userName);
                        if (QuyenUser != null)
                            Quyen.AddRange(QuyenUser);
                        if (Quyen.Count > 0)
                            _Quyen = Quyen.Distinct().ToArray();
                        if (_Quyen == null)
                            _Quyen[0] = "IsNull";
                        return _Quyen;
                    }
                }
                List<string> Quyen0 = new List<string>();
                Quyen0.Add("IsNull");
                return Quyen0.ToArray();
            }
            catch
            {
                List<string> Quyen = new List<string>();
                Quyen.Add("IsNull");
                return Quyen.ToArray();
            }
        }


        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}