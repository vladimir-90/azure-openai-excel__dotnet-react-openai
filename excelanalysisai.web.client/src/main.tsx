import 'bootstrap/dist/css/bootstrap.min.css';
import './index.scss';

import App from './App.tsx';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';

createRoot(document.getElementById('root')!).render(
	<StrictMode>
		<App />
	</StrictMode>
);
