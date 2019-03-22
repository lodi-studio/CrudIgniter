# CRUDIgniter
CRUDIgniter helps developer to create a less CRUD operations using MySql database, it is very easy to use and makes your code more readable also lessen the codes in your Data Access Layer

## Requirements 
> MySql Connector .Net https://downloads.mysql.com/archives/get/file/mysql-connector-net-6.9.9.msi

## Example

###### Table
```MySql
CREATE TABLE  `employee` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `Address` varchar(45) NOT NULL,
  `Email` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;
```

###### C# Example

```C#
namespace Model 
{
  public class Employee 
  {
    #region Constructor
    public Employee() {}
    
    public Employee(int id, string name, strin address, string email) 
    {
      this.Id = id;
      this.Name = name;
      this.Address = address;
      this.Email = email;
    }
     public Employee(string name, strin address, string email) 
    {
      this.Name = name;
      this.Address = address;
      this.Email = email;
    }
    #endregion

    #region Property
    public int Id { get;set; }
    public string Name { get;set; }
    public string Address { get;set }
    public string Email { get;set; }
    #endregion
  }
}

```
