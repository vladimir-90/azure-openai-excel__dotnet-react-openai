import Spinner from './Spinner';

interface LazySelectOption {
	value: string;
	label: string;
}

interface LazySelectProps {
	label: string;
	value: string;
	onChange: (value: string) => void;
	loading: boolean;
	options: LazySelectOption[];
	placeholder?: string;
}

function LazySelect({
	label,
	value,
	onChange,
	loading,
	options,
	placeholder = 'Choose option...',
}: LazySelectProps) {
	return (
		<div className="form-floating position-relative">
			<select
				value={value}
				onChange={(e) => onChange(e.target.value)}
				className="form-select question-form-select"
			>
				<option value="" disabled>
					{placeholder}
				</option>
				{options.map((option) => (
					<option key={option.value} value={option.value}>
						{option.label}
					</option>
				))}
			</select>
			{loading && (
				<div
					className="position-absolute"
					style={{
						right: '50px',
						top: '50%',
						transform: 'translateY(-50%)',
						pointerEvents: 'none',
					}}
				>
					<Spinner sm />
				</div>
			)}
			<label className="text-muted">{label}</label>
		</div>
	);
}

export default LazySelect;
