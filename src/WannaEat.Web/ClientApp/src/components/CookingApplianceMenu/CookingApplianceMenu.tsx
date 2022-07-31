import React, {FC, ReactEventHandler, useState} from 'react';
import {CookingAppliance} from "../../entities/cooking-appliance";
import './CookingApplianceMenu.tsx.css'
import {useToggle} from "../../hooks/useToggle";

export interface CookingApplianceMenuProps {
    appliances: CookingAppliance[],
    applianceChangeCallback: (selected: CookingAppliance[]) => (void)
}

const CookingApplianceMenu: FC<CookingApplianceMenuProps> = ({appliances}) => {
    const [selected, setSelected] = useState<CookingAppliance[]>([])
    const isSelected = (a: CookingAppliance) => selected.some(s => s.id === a.id);
    const [shouldShowDropDown, toggleDropDown] = useToggle();
    const anyApplianceId = -1;
    const onApplianceClick = (e: React.MouseEvent<HTMLLIElement, MouseEvent>) => {
        e.stopPropagation()
        const id = e.currentTarget.value;
        console.log(id)
    }
    
    const chooseAppliance = (id: number) => {
        if (selected.length != 0) {
            
        }
    }
    
    return (
        <div>
            <i title={'Чем готовить?'} onClick={() => toggleDropDown()} className={'fa-solid fa-kitchen-set fa-2xl'}></i>
            <div className={'bg-light rounded-1'} style={{
                zIndex: 9999,
                position: 'absolute',
                visibility: shouldShowDropDown ? 'visible' : 'hidden',
            }}>
                <div className={'bg-light p-1 rounded-1'}>
                    <ul className={'list-group'}>
                        <li className={'list-group-item'} value={anyApplianceId} onClick={onApplianceClick}
                            key={anyApplianceId}>Чем угодно
                        </li>
                        {
                            appliances.map(a => (
                                <li className={'list-group-item'} value={a.id} key={a.id}
                                    onClick={onApplianceClick}>{a.name}</li>
                            ))
                        }
                    </ul>
                </div>
            </div>
        </div>
    );
};

export default CookingApplianceMenu;