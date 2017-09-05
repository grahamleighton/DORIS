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


        public string getUserName()
        {
            return (_initialisationvalue == 23) ? _username : "";
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
        }

        public UserDetail(string UserName , string FullName , string SupplierCode,string SupplierName,string Hash)
        {
            _username = UserName;
            _fullname = FullName;
            _suppliercode = SupplierCode;
            _suppliername = SupplierName;
            _hash = Hash;
            _initialisationvalue = 23;
        }
        public void setDetails(string UserName, string FullName, string SupplierCode, string SupplierName,string Hash)
        {
            _username = UserName;
            _fullname = FullName;
            _suppliercode = SupplierCode;
            _suppliername = SupplierName;
            _initialisationvalue = 23;
            _hash = Hash;
        }

        public bool IsValid()
        {
            return (_initialisationvalue == 23);
        }
    }
}