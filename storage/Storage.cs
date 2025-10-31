using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAppTest
{
    interface Storage
    {
        void setupDataSource();
        void initDataBase();
        User getUSer(string username);
        void addUser(string username, string passwordHash);
        void removeUser(User user);
      

    }
}
