# .NetAPI

In the appsettings.json make sure to fill DefaultConnection and SigningKey values with your own values like this:

{

  "ConnectionStrings": { 
  
    "DefaultConnection": "{Add your database connection string here}"
    
  },
  
  "Logging": {
  
    "LogLevel": {
    
      "Default": "Information",
      
      "Microsoft.AspNetCore": "Warning"
      
    }
    
  },
  
  "AllowedHosts": "*",
  
  "JWT": {
	
     "Issuer": "http//localhost:5163",
     
     "Audience": "http//localhost:5163",
     
     "SigningKey": "{Add a string here for the signingkey}" 
     
	}  
}
