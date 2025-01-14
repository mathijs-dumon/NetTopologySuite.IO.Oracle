## Integration tests
### Requirements
In order to perform the tests, a Docker container with an Oracle XE database has to be available.
One that worked for us is [Oracle Express Edition 11g Release 2 on Ubuntu 18.04 LTS](https://hub.docker.com/r/oracleinanutshell/oracle-xe-11g):   
```
docker pull oracleinanutshell/oracle-xe-11g
```

Make sure you bind the correct port when running the image:
```
docker run -d -p 49161:1521 oracleinanutshell/oracle-xe-11g
```

You should be able to connect using the following values:
Property | Value
--- | ---
hostname | localhost
port | 49161
sid | xe
username | system
password | oracle

Make sure you change the connectionstring in the test project's App.config to match with whatever test database you want to run the tests against. 
The default settings assume the above docker image is running on localhost.

### Performing the tests
- Open the solution 'NetTopologySuite.IO.Oracle.sln'
- Go to Test > Test Explorer 
- Run the tests (should be all green)
