using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DORIS.Models
{
    public class UserDetail
    {
        private string _username;
        private string _fullname;
        private string _suppliercode;
        private string _suppliername;
        private int _initialisationvalue;
        private string _hash;
        private long _userid;


        public string getUserName()
        {
            return (_initialisationvalue == 23) ? _username : "";
        }
        public long getUserID()
        {
            return (_initialisationvalue == 23) ? _userid : 0;
        }
        public string getHash()
        {
            return (_initialisationvalue == 23) ? _hash : "";
        }
        public string getFullName()
        {
            return (_initialisationvalue == 23) ? _fullname : "";
        }
        public string getSupplierCode()
        {
            return (_initialisationvalue == 23) ? _suppliercode : "";
        }
        public string getSupplierName()
        {
            return (_initialisationvalue == 23) ? _suppliername : "";
        }

        public UserDetail()
        {
            _initialisationvalue = 0;
            _fullname = "";
            _suppliercode = "";
            _suppliername = "";
            _username = "";
            _hash = "";
            _userid = 0;
        }

        public UserDetail(string UserName, string FullName, string SupplierCode, string SupplierName, string Hash,long UserID)
        {
            _username = UserName;
            _fullname = FullName;
            _suppliercode = SupplierCode;
            _suppliername = SupplierName;
            _hash = Hash;
            _initialisationvalue = 23;
            _userid = UserID;
        }
        public void setDetails(string UserName, string FullName, string SupplierCode, string SupplierName, string Hash,long UserID)
        {
            _username = UserName;
            _fullname = FullName;
            _suppliercode = SupplierCode;
            _suppliername = SupplierName;
            _initialisationvalue = 23;
            _hash = Hash;
            _userid = UserID;
        }

        public bool IsValid()
        {
            return (_initialisationvalue == 23);
        }
        public UserDetail Export()
        {
            UserDetail n = new UserDetail();
            n._hash = this._hash;
            n._fullname = this._fullname;
            n._suppliercode = this._suppliercode;
            n._suppliername = this._suppliername;
            n._username = this._username;
            n._initialisationvalue = this._initialisationvalue;
            n._userid = this._userid;
            return n;

        }
        public void Import(UserDetail ud)
        {
            this._fullname = ud._fullname;
            this._hash = ud._hash;
            this._initialisationvalue = ud._initialisationvalue;
            this._suppliercode = ud._suppliercode;
            this._suppliername = ud._suppliername;
            this._username = ud._username;
            this._userid = ud._userid;
        }

    }
}