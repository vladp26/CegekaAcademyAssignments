import { Button, Card, CardActions, CardContent, CardMedia, Tooltip, Typography } from "@mui/material";
import { Fundraiser } from "../Models/Fundraiser";
import DoDisturbAltIcon from '@mui/icons-material/DoDisturbAlt';
import fundraiserPhoto from  '../Assets/Icons/fundraiser.png';
import Popup from 'reactjs-popup';
import './DonatePopup.css'
import { useEffect, useState } from "react";
import {Person} from "../Models/Person"

export interface IFundraiserCardProps {
    fundraiser: Fundraiser;
}
export interface IDonationDetails{
    donationAmount:number;
    person:Person
}

export const FundraiserCard = (props: IFundraiserCardProps) => {

    const [form, setForm] = useState<IDonationDetails>({
        donationAmount:0,
        person:new Person("anonim", "0000000000000", new Date())
      });
      const [isSending, setSending] = useState<boolean>(false);

      const handleChange = (event: React.FormEvent<HTMLInputElement>) => {
        const t=event.target as HTMLInputElement;
        setForm({...form,[t.name] : t.value});
      };
      const showData = () => {
        console.log('Form: ', form);
      }
      const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        // get donationAmount
        let donationAmount:number = Number((window.document.getElementById("donationAmount") as HTMLInputElement).value);
        form.donationAmount = donationAmount;
    
        // get person name
        let name:string = (window.document.getElementById("name") as HTMLInputElement).value;
        form.person.name = name;
    
        // get person idNumber
        let idNumber:string = (window.document.getElementById("idNumber") as HTMLInputElement).value;
        form.person.idNumber = idNumber;
        
        // get dateOfBirth
        let dateOfBirth:Date = new Date((window.document.getElementById("dateOfBirth") as HTMLInputElement).value);
        form.person.dateOfBirth = dateOfBirth;
    
        showData ();
    
      };
    return (
        
        <Card sx={{ maxWidth: 345 }}>
            
            <CardMedia
                sx={{ height: 140 }}
                title={props.fundraiser.name}
                image={fundraiserPhoto}
            />
            <CardContent>
                <Typography gutterBottom variant="h5" component="div">
                    {props.fundraiser.name} &nbsp; 
                    {
                       props.fundraiser.status==="Closed" &&
                        <Tooltip title={`${props.fundraiser.name} has already raised its goal. Yay!`}><DoDisturbAltIcon></DoDisturbAltIcon></Tooltip>
                    }
                </Typography>
                <Typography gutterBottom variant="h5" component="div" fontSize={12}>
                    Created by {props.fundraiser.owner.name} on {props.fundraiser.creationDate.toString().substring(0,10)}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    We need {props.fundraiser.goalValue} RON by {props.fundraiser.dueDate.toString().substring(0,10)}. We received {props.fundraiser.donationAmount} RON so far.
                </Typography>
                
            </CardContent>
            <CardActions sx={{ float: "right" }}>
                <p></p>
                {
                    props.fundraiser.status==="Closed"
                        ? null
                       : <Popup trigger={<Button size="small" color='primary' variant="contained">Donate</Button>} nested>
                        <form onSubmit={(e) => handleSubmit(e)}>
                        <p>PS: nu am reusit sa termin, nu trimite datele la baza de date :') </p>
                         <div>How much would you like to donate? 
                            
                         <input type='number' min='0' inputMode="numeric"
                            name="donationAmount" id="donationAmount" onChange={handleChange}
                            />
                         </div>
                         <div>What is your name?
                         <input type='text' required minLength={2}
                            name="name" id='name' onChange={handleChange}
                            />
                         </div>
                         <div>What is your idNumber?
                         <input type='text' placeholder="type 13 characters" required minLength={13} maxLength={13}
                            name="idNumber" id='idNumber' onChange={handleChange}
                            />
                         </div>
                         <div>What is your date of birth?
                         <input type='date' defaultValue="yyyy-mm-dd" 
                            name="dateOfBirth" id='dateOfBirth' onChange={handleChange}
                            />
                         </div>
                         </form>
                         <Button type="submit" size="small" color='primary' variant="contained">Send donation </Button>
                        </Popup>
                    
                }
            </CardActions>
        </Card>
    );
}
