# CRUDIgniter
CRUDIgniter helps developer to create a less CRUD operations using MySql database, it is very easy to use and makes your code more readable also lessen the codes in your Data Access Layer

## Dependencies 
> MySql Connector .Net 6.9.9/MySql.Data  https://downloads.mysql.com/archives/get/file/mysql-connector-net-6.9.9.msi

## Example

### Table
```MySql
CREATE TABLE  `employee` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `Address` varchar(45) NOT NULL,
  `Email` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;
```

### C# Example

#### Model
```C#
namespace Models 
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
    public string Address { get;set; }
    public string Email { get;set; }
    #endregion
  }
}
```

#### Data Access Layer
```C#
using System.Data;
using CrudIgniter.Helper;

namespace DAL 
{
  private CRUD crud = new CRUD("change your connection string here..");
  
  // Get all employee
  public DataTable GetAllEmployee()
  {
    return crud.Select("employee");
  }
  
  // Get employee using id
  public DataTable GetEmployeeById(int id)
  {
    string sql = "SELECT * FROM employee WHERE Id=?";
    
    return crud.GetData(sql, new object[] {id});
  }
  
  // Insert employee record
  public void Create(Models.Employee employeeModel)
  {
    crud.Table = "employee";
    crud.Columns = new string[] { "Name", "Address", "Email" };
    crud.Values = new object[] { employeeModel.Name, employeeModel.Address, employeeModel.Email };
    crud.Insert();
  }
  
  // Update employee record
  public void Update(Models.Employee employeeModel)
  {
    crud.Table = "employee";
    crud.Columns = new string[] { "Name", "Address", "Email" };
    crud.Values = new object[] { employeeModel.Name, employeeModel.Address, employeeModel.Email };
    crud.Where = new string[] { "Id" };
    crud.Where = new object[] { employeeModel.Id };
    crud.Update();
  }
  
  // Delete employee record
  public void Delete(int id)
  {
    crud.Table = "employee";
    crud.Where = new string[] { "Id" };
    crud.Where = new object[] { id};
    crud.Delete();
  }
}
```
#### Business Logic Layer
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CrudIgniter.Helper;

namespace BLL
{
  private DAL.Employee employeeDAL = new DAL.Employee();
  
  // convert datatable to list of object 'Employee'
  public List<Models.Employee> GetAllEmployee() 
  {
    return employeeDAL.GetAllEmployee().ToList<Models.Employee>();
  }
  
  // convert datarow to object
  public Models.Employee GetEmployeeById(int id)
  {
    Models.Employee employeeModel = new Models.Employee();
    
    foreach (DataRow row in employeeDAL.GetEmployeeById(id).Rows)
    {
      employeeModel = row.ToObject<Models.Employee>();
    }
    
    return employeeModel;
  }
  
  public void Create(Models.Employee employeeModel)
  {
    // you can add validation here
  
    employeeDAL.Create(employeeModel);
  }
  
  public void Update(Models.Employee employeeModel)
  {
    // you can add validation here
    
    employeeDAL.Update(employeeModel);
  }
  
  public void Delete(int id)
  {
    employeeDAL.Delete(id);
  }
}
```

#### UI/Console

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
  class Program
  {
        static void Main(string[] args)
        {

            BLL.Employee employeeBLL = new BLL.Employee();

            // Get all employee
            foreach (Models.Employee employeeModel in employeeBLL.GetAllEmployee())
            {
                Console.WriteLine("___________________________________");
                Console.WriteLine("ID: " + employeeModel.Id);
                Console.WriteLine("Name: " + employeeModel.Name);
                Console.WriteLine("Address: " + employeeModel.Address);
                Console.WriteLine("Email: " + employeeModel.Email);
                Console.WriteLine("___________________________________\n");
            }

            Console.ReadKey();
        }
  }
}
```
