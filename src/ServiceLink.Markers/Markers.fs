namespace ServiceLink

type EndpointKind = 
    | Notify
    | Cmd
    | Call

type IConfigurable = 
    abstract Configure : EndpointKind -> unit 

type Notify<'t> =
    abstract Configure<'config when 'config :> IConfigurable> : 'config -> unit 
 
type Command<'t> =
    abstract Configure<'config when 'config :> IConfigurable> : 'config -> unit
 
type Call<'arg, 'result> =
    abstract Configure<'config when 'config :> IConfigurable> : 'config -> unit

