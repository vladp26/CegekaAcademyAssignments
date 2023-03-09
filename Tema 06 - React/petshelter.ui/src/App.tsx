import './App.css';
import { Pets } from './Pages/Pets';
import { Route, Routes } from 'react-router-dom';
import { Home } from './Pages/Home';
import { NoMatch } from './Pages/NoMatch';
import { Layout } from './HOC/Layout';
import { Fundraisers } from './Pages/Fundraisers';

function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/Pets" element={<Pets></Pets>} />
        <Route path='/Fundraisers' element={<Fundraisers></Fundraisers>}/>
        <Route path="/" element={<Home></Home>} />
        <Route path="*" element={<NoMatch></NoMatch>} />
      </Routes>
    </Layout>
  );
}

export default App;
