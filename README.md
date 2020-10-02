# iSAMSDataExtract

## CLI Tool To Extract Data From iSAMS Using The Official API

Copy the iSAMSData.exe into a location on your device (e.g. c:\temp) and open a command line from the same place.

You need to pass in 6 parameters (listed below). These parameters are separated by a space (see below example).

**Required Parameters**

- Token URL (e.g. YOURSCHOOL.isams.cloud/auth/connect/token)
- Client ID (e.g. Your iSAMS key)
- Client Secret (e.g. Your iSAMS secrwet)
- API Endpoint URL (e.g. YOURSCHOOL.isams.cloud/api/humanresources/employees)
- JSON Element (e.g. employees)
- Data Save Location (e.g. c:\temp\results.json)

---

**CLI Example**

### isamsdata.exe **YOURSCHOOL.isams.cloud/auth/connect/token** **YOURKEY** **YOURSECRET** **YOURSCHOOL.isams.cloud/api/humanresources/employees** **employees** **C:\temp\Data.json**

---

Results are a JSON format text file, which can be used for Power Automate, Excel, Power BI, PowerShell Scripts, Database Importing or even another program. The advantage with a console app is that this can be very easily run via task scheduler (e.g. a nightly update of data etc)

---

### Please Note: Ensure you are fully aware of where your data is being downloaded to and make certain this is **always** secure and deleted when no longer required.
