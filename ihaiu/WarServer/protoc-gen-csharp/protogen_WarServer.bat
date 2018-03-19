@protoc -I./../proto ./../proto/*.proto -oPackets.bin 
cd clientgen
protogen.exe -i:../Packets.bin -o:./../../Sources/ProtoPacket_Master.cs -ns:Games.PB -p:detectMissing
::@handlegen ..\Packets.cs .. S ..\ProtoMap.cs
::@typemapgen ..\Packets.cs C ..\PacketTypeMap.cs
cd ../