#Ufr Plugin

## Usage:

### Pre-defined Scene

You can simply additively load the SmartObjectTestScene at any point, and you will be presented with the Configuration UI (which is displayed the first time it starts)
Once configured, the Serial Numbers and SmartObjectConnectors will be saved and you won't see the UI again.

If you want, you can run "Edit -> Clear All PlayerPrefs" from the Unity Editor window to run the configuration again.

OR

You can manually call ReConfigure on the SmartObjectManager


#### Manual Setup

Add a SmartObjectManager to a gameobject in the scene.
Modify SmartObjectDefinitions.cs to add any new SmartObject types
Modify the list of Smart Objects to configure on the SmartObjectManager inspector window
Drag a reference to a CanvasGroup, which contains at least 1 Text node to display the current smart object type you're trying to configure.


### Accessing a connector

Once the SmartObjectManager is configured, you can get access to each instance of a SmartObjectConnector using the following syntax

```
private async void GetConnectors() {
	SmartObjectManager manager = SmartObjectManager.Instance;
	if (manager != null) {
	  SmartObjectConnector legoStoreConnector = await manager.GetSmartConnector(SmartObjectType.LegoStore);
	  SmartObjectConnector magicPadConnector = await manager.GetSmartConnector(SmartObjectType.MagicPad);
	  SmartObjectConnector magicPrinterConnector = await manager.GetSmartConnector(SmartObjectType.MagicPrinter);

	  // Then, to receive a callback when a connector has just read data (e.g. an NFC chip)
	  legoStoreConnector.Connect(this.OnReadConnector);
	  magicPadConnector.Connect(this.OnReadConnector);
	  magicPrinterConnector.Connect(this.OnReadConnector);
	}
}

private void OnReadConnector(string data, SmartObjectType type) {
	switch (type)
	{
		case SmartObjectType.LegoStore:
			TriggerLegoStoreLogicWithNFCData(data);
			break;
		case SmartObjectType.MagicPad:
			TriggerLegoStoreLogicWithNFCData(data);
			break;
		case SmartObjectType.MagicPrinter:
			TriggerLegoStoreLogicWithNFCData(data);
			break;
		default:
			break;
	}
}

```

