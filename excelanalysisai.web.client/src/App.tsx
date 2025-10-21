import {
	getAvailableAiModels,
	getTestDataSets,
} from './services/generation-settings.service';
import { useEffect, useState } from 'react';

import QuestionForm from './components/QuestionForm';

function App() {
	const [aiModels, setAiModels] = useState<string[]>([]);
	const [testDataSets, setTestDataSets] = useState<string[]>([]);
	const [loading, setLoading] = useState<boolean>(true);

	useEffect(() => {
		loadData();
	}, []);

	const loadData = async () => {
		setLoading(true);

		const [models, dataSets] = await Promise.all([
			getAvailableAiModels(),
			getTestDataSets(),
		]);

		setAiModels(models);
		setTestDataSets(dataSets);

		setLoading(false);
	};

	if (loading) {
		return (
			<div
				className="container d-flex justify-content-center align-items-center"
				style={{ minHeight: '100vh' }}
			>
				<div className="p-3">Loading...</div>
			</div>
		);
	}

	return (
		<div
			className="container d-flex justify-content-center align-items-center"
			style={{ minHeight: '100vh' }}
		>
			<QuestionForm aiModels={aiModels} testDataSets={testDataSets} />
		</div>
	);
}

export default App;
