namespace ServiceLink
open System
open System.Reflection

type InfoProvider = internal InfoProvider of (Type -> obj list)

type InfoProviderError =
    | NotFound 
    | MoreThanOne

module InfoProvider =
    let private isAssignable (t1 : Type) (t2 : Type) =
        let ti1 = t1.GetTypeInfo()
        let ti2 = t2.GetTypeInfo()
        ti1.IsAssignableFrom(ti2)

    let getByType ip typ =
        let (InfoProvider p) = ip in (p typ) 
    
    let get<'t> ip =
        typeof<'t> |> getByType ip |> List.map (fun p -> p :?> 't)
    
    let singleByType ip typ =
        match getByType ip typ with
        | [] -> Error NotFound
        | [v] -> Ok v
        | _ -> Error MoreThanOne

    let single<'t> ip =
        singleByType ip typeof<'t> |> Result.map (fun k -> k :?> 't)

    let create<'t> provide = 
        fun t  ->
            if (isAssignable typeof<'t> t) then 
                provide() |> List.map (fun (p: 't) -> p :> obj)
            else
                List.empty
        |> InfoProvider

    let createFrom (v :'t) =
        fun t ->
            if (isAssignable typeof<'t> t) then [v :> obj] else []
        |> InfoProvider

    let orElse p1 p2 =
        fun t -> 
            let p1r = getByType p1 t
            match p1r with
            | [] -> getByType p2 t
            | _ -> p1r
        |> InfoProvider
    
    let join p1 p2 =
        fun t -> 
            getByType p1 t |> List.append (getByType p2 t)
        |> InfoProvider

    let assignValue p1 v =
        createFrom v |> orElse p1
    let assign p1 fv =
        create fv |> orElse p1

    let map p (f : 'a -> 'b) =
        assign p (fun () -> get<'a> p |> List.map f)
        
    let fromMemberAttributes (mi : MemberInfo) = 
        create (fun () -> mi.GetCustomAttributes() |> Seq.toList)
