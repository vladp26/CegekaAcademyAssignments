export class Person {
    idNumber: string;
    name: string;
    //contactNumber: string;
    dateOfBirth:Date

    constructor( name: string,idNumber: string,dateOfBirth:Date) {
        this.idNumber = idNumber;
        this.name = name;
        //this.contactNumber = contactNumber;
        this.dateOfBirth=dateOfBirth;
    }
}