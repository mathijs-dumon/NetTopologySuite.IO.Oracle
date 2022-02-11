## Usage

### Writing example
```C#
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Sdo;
using Oracle.ManagedDataAccess.Client;
using System.Data;

...

  // Create a NTS geometry object from WKT - or get it in another way
  var wr = new WKTReader();
  Geometry geom = wr.Read(" wkt goes here ");

  // Write NTS geometry to an Oracle UDT object:
  var oracleWriter = new OracleGeometryWriter();
  SdoGeometry udt = oracleWriter.Write(geom);

  // Open connection
  using var con = new OracleConnection("your-connectionstring-goes-here");
  con.Open();

  // Adapt query to suit your needs
  var queryString = $"INSERT INTO TEST_TABLE (data) VALUES (:geo)";
  using OracleCommand command = new OracleCommand(queryString, con);

  var geometryParam = new OracleParameter()
  {
      Direction = ParameterDirection.Input,
      ParameterName = "geo", // needs to match with the parameter name in the query
      Value = udt,
      DbType = DbType.Object, // this is important!
      UdtTypeName = "MDSYS.SDO_GEOMETRY" // so is this!
  };
  command.Parameters.Add(geometryParam);
  command.ExecuteNonQuery();
  
...

```

### Reading example
```C#
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Sdo;
using Oracle.ManagedDataAccess.Client;
using System.Data;

...

  // Open connection
  using var con = new OracleConnection("your-connectionstring-goes-here");
  con.Open();

  // Adapt query to suit your needs
  var queryString = $"SELECT data FROM TEST_TABLE";
  using OracleCommand command = new OracleCommand(queryString, con);

  // Execute query & cast the column value
  var res = (SdoGeometry) command.ExecuteScalar();

  // Convert the UDT object to an NTS geometry:
  var oracleReader = new OracleGeometryReader();
  var geom = oracleReader.Read(res);
  
...

```

## Integration tests
### Requirements
In order to perform the tests, a Docker container with an Oracle XE database has to be available.
One that worked for us is [oci-oracle-xe](https://hub.docker.com/r/gvenzl/oracle-xe):   
```
docker pull docker pull gvenzl/oracle-xe:latest
```

Make sure you bind the correct port when running the image:
```
docker run -d -p 1521:1521 -e ORACLE_PASSWORD=secret gvenzl/oracle-xe
```

You should be able to connect using the following values:
Property | Value
--- | ---
hostname | localhost
port | 1521
sid | xe
username | system
password | oracle

Make sure you change the connectionstring in the test project's App.config to match with whatever test database you want to run the tests against. 
The default settings assume the above docker image is running on localhost.

### Performing the tests
- Open the solution 'NetTopologySuite.IO.Oracle.sln'
- Go to Test > Test Explorer 
- Run the tests (should be all green)

The integration test is fairly simple: it creates a table with a geometry column, writes a row, reads it back and compares the two 
versions for equality. After that it drops the table again.
You should be able to run this against any database, the table name created and dropped is defined at the tope of the test class  (="NTS_TEST_GEO_DATA").
