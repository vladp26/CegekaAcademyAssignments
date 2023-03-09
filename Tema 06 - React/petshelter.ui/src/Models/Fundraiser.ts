import type { Person } from "./Person";

export class Fundraiser {
    id:number;
    name: string;
    owner: Person;
    goalValue: number;
    dueDate: Date;
    status: string;
    creationDate: Date;
    donationAmount: number;
    donors: Person[]

    constructor(name: string,
        owner: Person,
        goalValue: number,
        dueDate: Date,
       // status: string,
       // creationDate: Date,
       // donationAmount: number,
        //donors: Person[]
        ) {
            this.id=Math.floor(Math.random() * 100000);
            this.name=name;
            this.owner=owner;
            this.goalValue=goalValue;
            this.dueDate=dueDate;
            this.creationDate=new Date()
            if(this.dueDate>this.creationDate)
            {
                this.status="Active";
            }
            else
            {
                this.status="Closed";
            }
            this.donationAmount=0;
            this.donors=[];
    }
    static createFundraiserWithAllAttributes(name: string,
        owner: Person,
        goalValue: number,
        dueDate: Date,
       status: string,
       creationDate: Date,
       donationAmount: number,
        donors: Person[]
        ):Fundraiser {
            let fundraiser=new Fundraiser(name, owner, goalValue, dueDate)
            fundraiser.creationDate=creationDate
            fundraiser.status=status
            fundraiser.donationAmount=donationAmount
            fundraiser.donors=donors
            return fundraiser;
    }
}
