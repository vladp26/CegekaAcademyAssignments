import axios from 'axios';
import { Person } from '../models/Person';
import { Fundraiser } from '../models/Fundraiser';

export class FundraiserService
{
    private apiUrl: string = "http://localhost:5009";
    public getAll(): Promise<Fundraiser[]> {
        return axios
        .get(this.apiUrl + '/Fundraiser')
        .then(response => {
            //console.log(response.data);

            var fundraiserResponse: Fundraiser[] = [];
            response.data.forEach((fundraiserFromApi:Fundraiser) => {

                let owner = new Person(fundraiserFromApi.owner.name, fundraiserFromApi.owner.id);
                fundraiserFromApi.owner=owner;
                fundraiserResponse.push(fundraiserFromApi);
            });

            return fundraiserResponse;
        })
    }
    public addFundraiser(fundraiser: Fundraiser): Promise<void> {
        //let owner:PersonDto
        let fundraiserToAdd: FundraiserDto= {
            name: fundraiser.name,
            dueDate :fundraiser.dueDate,
            goalValue:fundraiser.goalValue,
            owner : {
                dateOfBirth : new Date(),
                idNumber : fundraiser.owner.id,
                name : fundraiser.owner.name
            },
        }

        return axios.post(this.apiUrl + '/Fundraiser', fundraiserToAdd);
    }
    public getById(id: number):Promise<Fundraiser>
    {
    return axios.get(this.apiUrl + `/Fundraiser/${id}`)
        .then(response => {
            let fundraiserResponse:Fundraiser=
            Fundraiser.createFundraiserWithAllAttributes(response.data.name, response.data.owner, response.data.goalValue, 
                 response.data.dueDate, response.data.status, response.data.creationDate,response.data.donationAmount, response.data.donors);
            return fundraiserResponse;
    })
    }

    public deleteFundraiser(id: number):Promise<void>
    {
        return axios.put(this.apiUrl + `/Fundraiser/${id}/delete`);
    }

    public donate(fundraiserId: number, donorId: number, amount:number):Promise<void>
    {
        let donation:any ={amount:amount}
        return axios.put(this.apiUrl + `/Fundraiser/${fundraiserId}/donate/${donorId}`, donation);
    }
}

interface PersonDto
{
    name: string; 
    dateOfBirth: Date | undefined; 
    idNumber: string; 
}

interface FundraiserDto
{
    name:string;
    owner: PersonDto;
    goalValue: number;
    dueDate:Date;
}

