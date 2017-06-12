Login-AzureRmAccount -Environment AzureUSGovernment

New-AzureRmResourceGroup -Name SecureAppNetRG1 -Location "USGov Virginia"
New-AzureRmResourceGroupDeployment -Name SecureAppNetDeployment -ResourceGroupName SecureAppNetRG1 `
  -TemplateFile C:\Users\nmgiard\Desktop\SecureNetworkTemplate\SecureAppNetwork.json
