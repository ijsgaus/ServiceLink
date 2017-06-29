namespace ServiceLink.Markers

type ICallable<'args, 'result> =
    abstract ConfigureRequest<'t> : 't * 'args -> 't
    abstract ConfigureResponse<'t> : 't * 'result -> 't
    
type ICommand<'command> =
    abstract Configure<'t> : 't * 'command -> 't

type INotify<'message> =
    abstract Configure<'t> : 't * 'message -> 't