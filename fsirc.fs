open System
open System.IO
open System.Net
open System.Net.Sockets
 
Console.WriteLine("server address?")
let server = Console.ReadLine()
Console.WriteLine("Portnum?")
let port  = int(Console.ReadLine())
Console.WriteLine("Channel?")
let channel = Console.ReadLine()
Console.WriteLine("nickname?")
let nick = Console.ReadLine()
 
let cri_client = new TcpClient();
cri_client.Connect( server, port )
 
let cri_reader = new StreamReader( cri_client.GetStream() )
let cri_writer = new StreamWriter( cri_client.GetStream() )
 
cri_writer.WriteLine( sprintf "USER %s %s %s %s" nick nick nick nick )
cri_writer.AutoFlush <- true
cri_writer.WriteLine( sprintf "NICK %s" nick )
cri_writer.WriteLine( sprintf "JOIN %s\n" channel )
 
let cri_ping ( writer : StreamWriter ) (pingmsg : string) =
  let pongmsg = pingmsg.[..0] + "O" + pingmsg.[2..] in
  writer.WriteLine(pongmsg)
 
let cri_privmsg ( writer : StreamWriter ) ( phrase : string ) =
  writer.WriteLine( sprintf "PRIVMSG %s :%s" channel phrase )
  
let cri_get_msg ( line : string )=
  line.Substring( line.Substring(1).IndexOf(":") + 2)

let cri_work = async {
  while( cri_reader.EndOfStream = false ) do
    let line = cri_reader.ReadLine()
    if line.StartsWith("PING") then
      cri_ping cri_writer line
    let (msg:string) = cri_get_msg line
    Console.WriteLine( line )}

do cri_work |> Async.Start
 
while true do
  let inpmsg = Console.ReadLine()
  if inpmsg <> "" then
    cri_privmsg cri_writer inpmsg


