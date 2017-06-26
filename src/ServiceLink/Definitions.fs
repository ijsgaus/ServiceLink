namespace ServiceLink

module rec Definitions =
    type Store = Store
    type HolderId = HolderId
    type Consumer = {
        Holder : HolderId
        Store : Store
    }
    type Supplier = {
        Holder : HolderId
        Store : Store
    }

    type ConstractName = ContractName
    type Contract = {
        ContractName : ConstractName
    }

    type Transport = Transport

    type EndpointName = EndpointName
    type Event = Event
    type Call = Call
    type TransportChannel = {
        Transport : Transport
    }
        
    type EndpointType =
        | Event of Contract * Transport option
        | Call  
    type Endpoint = {
        Name : EndpointName
        Type : EndpointType
    }
        

    

    type ServiceName = ServiceName
    type Service = {
        ServiceName : ServiceName
        Transport : Transport
        Endpoints : Endpoint list
    }
    
    
    
    type Serializer = Serializer