import React, {FC, useEffect, useState} from 'react';
import {CookingAppliance} from "../../entities/cooking-appliance";
import './CookingApplianceMenu.tsx.css'
import {useToggle} from "../../hooks/useToggle";

export interface CookingApplianceMenuProps {
    appliances: CookingAppliance[],
    applianceChangeCallback: (selected: CookingAppliance[]) => (void)
}

const CookingApplianceMenu: FC<CookingApplianceMenuProps> = ({appliances}) => {
    const [selected, setSelected] = useState<CookingAppliance[]>([])
    const [shouldShowDropDown, toggleDropDown] = useToggle();
    const isSelected = (a: CookingAppliance) => selected.some(s => s.id === a.id);
    const anyApplianceId = -1;
    
    const onApplianceClick = (e: React.ChangeEvent<HTMLInputElement>) => {
        const id = Number(e.currentTarget.value)
        if (!Number.isInteger(id)) {
            console.error('Given appliance id is not integer', {id});
            return;
        }
        const clicked = appliances.filter(a => a.id === id)[0];
        if (!clicked) {
            console.warn('Appliance with given id does not exist in appliance array', {id})
            return;
        }

        const alreadySelected = !e.currentTarget.checked;
        if (alreadySelected) {
            setSelected(selected.filter(s => s.id !== clicked.id))
        } else {
            setSelected([...selected, clicked])
        }
    }
    
    const onAnyApplianceClick = () => {
        setSelected([])
    }

    return (
        <div>
            <i title={'Чем готовить?'} onClick={() => toggleDropDown()} className={'fa-solid fa-kitchen-set fa-2xl'}></i>
            <div className={'bg-light rounded-1'} style={{
                zIndex: 9,
                position: 'absolute',
                visibility: shouldShowDropDown ? 'visible' : 'hidden',
            }}>
                <div onClick={toggleDropDown} style={{
                    position: 'fixed',
                    height: '100vh',
                    width: '100vw',
                    left: 0,
                    top: 0,
                    zIndex: -1,
                }}/>
                <div key={anyApplianceId} className={'bg-light p-1 rounded-1'}>
                    <div className={'form-check'} onClick={onAnyApplianceClick}>
                        <label className={'form-label'}>
                            <input checked={selected.length === 0} readOnly={true} value={anyApplianceId} type="checkbox" className={'form-check-input'}/>
                            Неважно
                        </label>
                    </div>
                    {
                        appliances.map(a => (
                            <div key={a.id} className="form-check">
                                <label className="form-label">
                                    <input onChange={onApplianceClick} readOnly={true} checked={isSelected(a)} value={a.id} type={'checkbox'} className="form-check-input"/>
                                    {a.name}
                                </label>
                            </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default CookingApplianceMenu;