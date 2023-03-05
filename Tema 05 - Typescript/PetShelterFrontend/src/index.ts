import { Fundraiser } from './models/Fundraiser';
import { Person } from './models/Person';
import { Pet } from './models/Pet';
import { FundraiserService } from './services/FundraiserService';
import { PetService } from './services/PetService';

let service = new PetService();

var petToRescue = new Pet(
    "Maricel", 
    "https://i.imgur.com/AO6wMYS.jpeg",
    "Cat",
    "AAAAA",
    new Date(),
    8,
    new Person("Costel", "1234567890123")
)

service.rescue(petToRescue)
    .then(() => 
        service.getAll()
        .then(pets => console.log(pets))
    );

let fundraiserService=new FundraiserService()
fundraiserService.getAll().then(result => console.log(result))

let owner:Person=new Person("nume owner", "1234567890127")
let fundraiserToAdd:Fundraiser= new Fundraiser("nume fundraiser", owner, 1000,new Date("2023-04-04T17:45:04.854Z") )
fundraiserService.addFundraiser(fundraiserToAdd)
console.log(fundraiserToAdd)



let fundraiserWithId6:Promise<Fundraiser>=fundraiserService.getById(6)
fundraiserWithId6.then(result => console.log(result))

fundraiserService.deleteFundraiser(10)

fundraiserService.donate(6,8,100)