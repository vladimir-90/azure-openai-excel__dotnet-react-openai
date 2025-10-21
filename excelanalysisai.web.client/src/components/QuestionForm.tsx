import { useState } from 'react';

interface QuestionFormProps {
	aiModels: string[];
	testDataSets: string[];
}

function QuestionForm({ aiModels, testDataSets }: QuestionFormProps) {
	const [selectedModel, setSelectedModel] = useState<string>('');
	const [selectedDataSet, setSelectedDataSet] = useState<string>('');
	const [question, setQuestion] = useState<string>('');

	const handleGoClick = () => {
		// to_do
	};

	return (
		<div
			className="card border-0 shadow-lg"
			style={{ maxWidth: 650, minWidth: 450 }}
		>
			{/* Header */}
			<div
				className="card-header bg-gradient text-center py-3 border-0"
				style={{
					background:
						'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
				}}
			>
				<h4 className="mb-0 fw-light">Excel Analysis AI</h4>
				<small className="opacity-75">
					Ask questions about your data
				</small>
			</div>

			{/* Form Body */}
			<div className="card-body p-4">
				<div className="row g-3">
					{/* Model Selection */}
					<div className="col-md-6">
						<div className="form-floating">
							<select
								id="model-select"
								value={selectedModel}
								onChange={(e) =>
									setSelectedModel(e.target.value)
								}
								className="form-select"
								style={{ height: '60px' }}
							>
								<option value="" disabled>
									Choose model...
								</option>
								{aiModels.map((model) => (
									<option key={model} value={model}>
										{model}
									</option>
								))}
							</select>
							<label
								htmlFor="model-select"
								className="text-muted"
							>
								🤖 AI Model
							</label>
						</div>
					</div>

					{/* Dataset Selection */}
					<div className="col-md-6">
						<div className="form-floating">
							<select
								id="dataset-select"
								value={selectedDataSet}
								onChange={(e) =>
									setSelectedDataSet(e.target.value)
								}
								className="form-select"
								style={{ height: '60px' }}
							>
								<option value="" disabled>
									Choose dataset...
								</option>
								{testDataSets.map((dataSet) => (
									<option key={dataSet} value={dataSet}>
										{dataSet}
									</option>
								))}
							</select>
							<label
								htmlFor="dataset-select"
								className="text-muted"
							>
								📊 Dataset
							</label>
						</div>
					</div>

					{/* Question Input */}
					<div className="col-12 mt-4">
						<div className="form-floating">
							<textarea
								id="question-textarea"
								value={question}
								onChange={(e) => setQuestion(e.target.value)}
								className="form-control"
								placeholder="What would you like to know about your data?"
								style={{ height: '120px', resize: 'vertical' }}
							/>
							<label
								htmlFor="question-textarea"
								className="text-muted"
							>
								💭 Your Question
							</label>
						</div>
						<div className="form-text text-muted mt-2">
							Try asking: "What are the top 5 sales by region?" or
							"Show me the average salary by department"
						</div>
					</div>
				</div>
			</div>

			{/* Footer with Action Button */}
			<div className="card-footer bg-light border-0 text-end p-4">
				<button
					onClick={handleGoClick}
					className="btn btn-lg px-5 text-white fw-semibold"
					disabled={
						!selectedModel || !selectedDataSet || !question.trim()
					}
					style={{
						background:
							selectedModel && selectedDataSet && question.trim()
								? 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)'
								: '#6c757d',
						border: 'none',
						borderRadius: '25px',
						transition: 'all 0.3s ease',
					}}
				>
					{!selectedModel || !selectedDataSet || !question.trim()
						? '✨ Complete the form above'
						: '🚀 Analyze Data'}
				</button>
			</div>
		</div>
	);
}

export default QuestionForm;
