## Remote-camera
A companion code for my MSDN articles published in [June 2017](https://msdn.microsoft.com/magazine/mt809116) and [November 2017](https://msdn.microsoft.com/magazine/mt845618)

## Notes
Please be advised that in order to make the Cloud tab working you need to specify hostname and device key to your IoT Hub. You do this by editing `Hostname` and `DeviceKey` properties of the Configuration class:

```
public static string Hostname { get; } = "<TYPE_YOUR_HOSTNAME_HERE>";        
public static string DeviceKey { get; } = "<TYPE_YOUR_KEY_HERE>";
```
