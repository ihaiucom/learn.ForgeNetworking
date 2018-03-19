@protoc -I./../proto ./../proto/*.proto -oPackets.bin 
mono ./clientgen/protogen.exe -i:Packets.bin -o:./../Sources/ProtoPacket_Master.cs -ns:Games.PB -p:detectMissing