import { Fragment, useState } from "react";
import { Fundraiser } from "../Models/Fundraiser";
import { FundraiserService } from "../Services/FundraiserService"
import { Box, Button, Container, Grid } from "@mui/material";
import { Link } from "react-router-dom";
import { FundraiserCard } from "../Components/FundraiserCard";
import { DonatePopup } from "../Components/DonatePopup";

export const Fundraisers = () =>{
    const fundraiserService = new FundraiserService()
    const [fundraisers, setFundraisers] = useState<Array<Fundraiser>>([])
    fundraiserService.getAll().then(result =>{
        setFundraisers(result)
    })
    
    return (
        <Fragment>
            <Box>
                <Button>
                    <Link to="/">Go to the home page</Link>
                </Button>
            </Box>
            <Container>

                <Grid container spacing={4}>
                    {
                        fundraisers.map((fundraiser) => (
                            <Grid item key={fundraiser.id} xs={12} sm={6} md={4}>
                                <FundraiserCard fundraiser={fundraiser}></FundraiserCard>
                            </Grid>
                        ))
                    }
                </Grid>
            </Container>
        </Fragment>
    );

}